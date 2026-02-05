import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { RentalService } from '../../services/rental.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-my-reservations',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './my-reservations.component.html'
})
export class MyReservationsComponent implements OnInit {

  rental: any;
  loading = true;
  error: string | null = null;

  constructor(
    private rentalService: RentalService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const user = this.authService.getUser();
    if (!user) {
      this.error = 'Usuario no autenticado';
      this.loading = false;
      return;
    }

    this.rentalService.getActiveRental(user.id).subscribe({
      next: data => {
        this.rental = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'No tenés reservas activas';
        this.loading = false;
      }
    });
  }
}

