import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CarService } from '../../services/car.service';
import { Car } from '../../models/car.model';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-car-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './car-list.component.html',
  styleUrl: './car-list.component.css'
})
export class CarListComponent implements OnInit {
  cars: Car[] = [];
  loading = false;
  error: string | null = null;

  constructor(
    private carService: CarService,
    public authService: AuthService
  ) { }


  ngOnInit(): void {
    this.loadCars();
  }


  loadCars(): void {
    this.loading = true;
    this.error = null;

    this.carService.getCars().subscribe({
      next: (data) => {
        this.cars = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar los autos. Por favor, verifica que el backend esté ejecutándose.';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }
  getCarImage(car: any): string {
    const key = `${car.brand}-${car.model}`
      .toLowerCase()
      .replace(/\s+/g, '');

    const map: Record<string, string> = {
      'bmw-m3': 'assets/cars/bmw-m3.jpg',
      'mercedesbenz-e220': 'assets/cars/mercedes-e320.jpg',
      'ford-fiesta': 'assets/cars/ford-m3.jpg'
    };

    return map[key] || 'assets/cars/default-car.jpg';
  }


  scrollToCatalog(): void {
    const el = document.getElementById('catalogo');
    if (el) {
      el.scrollIntoView({ behavior: 'smooth' });
    }
  }

}
