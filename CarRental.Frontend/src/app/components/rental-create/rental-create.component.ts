import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { CarService } from '../../services/car.service';
import { RentalService } from '../../services/rental.service';
import { Car } from '../../models/car.model';
import { RentalDto } from '../../models/rental.model';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-rental-create',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './rental-create.component.html',
  styleUrl: './rental-create.component.css'
})
export class RentalCreateComponent implements OnInit {
  car: Car | null = null;
  carId: number = 0;
  availableFrom: Date | null = null;
  loadingAvailability = false;
  rental: RentalDto = {
    userId: 0, // se setea desde AuthService
    carId: 0,
    startDate: new Date(),
    endDate: new Date()
  };


  loading = false;
  error: string | null = null;
  success = false;
  totalPrice: number = 0;

  constructor(
    private carService: CarService,
    private rentalService: RentalService,
    private authService: AuthService,
    public router: Router,
    private route: ActivatedRoute
  ) { }


  ngOnInit(): void {

    const user = this.authService.getUser();

    if (!user) {
      this.router.navigate(['/login']);
      return;
    }

    this.rental.userId = user.id;

    this.carId = Number(this.route.snapshot.paramMap.get('id'));
    this.rental.carId = this.carId;

    this.loadCar();
    this.loadAvailability();
  }

  loadCar(): void {
    this.loading = true;
    this.carService.getCar(this.carId).subscribe({
      next: (data) => {
        this.car = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar el auto.';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  loadAvailability(): void {
    this.loadingAvailability = true;

    this.rentalService.getCarAvailability(this.carId).subscribe({
      next: (data) => {
        this.availableFrom = data.availableFrom
          ? new Date(data.availableFrom)
          : null;
        this.loadingAvailability = false;
      },
      error: () => {
        this.availableFrom = null;
        this.loadingAvailability = false;
      }
    });
  }

  days = 0;
  today = new Date().toISOString().split('T')[0];

  calculateDays(): number {
    if (!this.rental.startDate || !this.rental.endDate) {
      return 0;
    }

    const start = this.parseLocalDate(this.rental.startDate);
    const end = this.parseLocalDate(this.rental.endDate);

    if (end <= start) {
      return 0;
    }

    const diffTime = end.getTime() - start.getTime();
    return Math.ceil(diffTime / (1000 * 60 * 60 * 24));
  }

  calculateTotalPrice(): number {
    if (this.car && this.days > 0) {
      return this.car.price * this.days;
    }
    return 0;
  }


  onSubmit(): void {
    if (!this.validateDates()) {
      return;
    }

    this.loading = true;
    this.error = null;
    this.success = false;

    // Convertir fechas a formato ISO
    const rentalData: RentalDto = {
      ...this.rental,
      startDate: new Date(this.rental.startDate),
      endDate: new Date(this.rental.endDate)
    };

    this.rentalService.createRental(rentalData).subscribe({
      next: (result) => {
        if (result.success) {
          this.success = true;
          this.totalPrice = result.totalPrice || 0;
          this.loading = false;

          // Redirigir después de 3 segundos
          setTimeout(() => {
            this.router.navigate(['/']);
          }, 3000);
        } else {
          this.error = result.error || 'Error al crear el alquiler.';
          this.loading = false;
        }
      },
      error: (err: any) => {
        this.error = err.error || 'Error al crear el alquiler. Por favor, verifica los datos.';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  private parseLocalDate(date: string | Date): Date {
    if (date instanceof Date) {
      return new Date(date.getFullYear(), date.getMonth(), date.getDate());
    }

    const [year, month, day] = date.split('-').map(Number);
    return new Date(year, month - 1, day);
  }

  validateDates(): boolean {
    if (!this.rental.startDate || !this.rental.endDate) {
      return false;
    }

    const start = this.parseLocalDate(this.rental.startDate);
    const end = this.parseLocalDate(this.rental.endDate);

    const today = new Date();
    today.setHours(0, 0, 0, 0);

    if (start < today) {
      this.error = 'La fecha de inicio no puede ser anterior a hoy.';
      return false;
    }

    if (this.availableFrom && start < this.availableFrom) {
      this.error = `El auto está disponible a partir del ${this.availableFrom.toLocaleDateString()}.`;
      return false;
    }

    if (end <= start) {
      this.error = 'La fecha de fin debe ser posterior a la fecha de inicio.';
      return false;
    }

    return true;
  }

  get minStartDate(): string {
    if (this.availableFrom) {
      return this.availableFrom.toISOString().split('T')[0];
    }
    return this.today;
  }


  get minEndDate(): string {
    if (!this.rental.startDate) {
      return this.today; // como mínimo hoy
    }

    const start = new Date(this.rental.startDate);
    start.setDate(start.getDate() + 1);
    return start.toISOString().split('T')[0];
  }


  onDateChange(): void {
    this.error = null;
    this.days = this.calculateDays();
    this.totalPrice = this.calculateTotalPrice();
  }



}
