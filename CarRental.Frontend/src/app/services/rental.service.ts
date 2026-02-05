import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { RentalDto, RentalResult } from '../models/rental.model';

@Injectable({
  providedIn: 'root'
})
export class RentalService {
  private apiUrl = 'http://localhost:5015/api/rentals';

  constructor(private http: HttpClient) { }

  createRental(rental: RentalDto): Observable<RentalResult> {
    return this.http.post<{ id: number; totalPrice: number }>(this.apiUrl, rental).pipe(
      map(response => ({
        success: true,
        rental: { id: response.id } as any,
        totalPrice: response.totalPrice
      })),
      catchError((error: HttpErrorResponse) => {
        let errorMessage = 'Error desconocido';
        if (error.status === 404) {
          errorMessage = 'Auto no encontrado.';
        } else if (error.status === 409) {
          errorMessage = 'El auto no está disponible para el período seleccionado.';
        } else if (error.error && typeof error.error === 'string') {
          errorMessage = error.error;
        }
        return throwError(() => ({
          success: false,
          error: errorMessage
        }));
      })
    );
  }
  getActiveRentalsByCar(carId: number): Observable<{ startDate: string; endDate: string }[]> {
    return this.http.get<{ startDate: string; endDate: string }[]>(
      `${this.apiUrl}/car/${carId}`
    );
  }

  getCarAvailability(carId: number): Observable<{ availableFrom: string }> {
    return this.http.get<{ availableFrom: string }>(
      `${this.apiUrl}/car/${carId}/availability`
    );
  }

  getActiveRental(userId: number) {
    return this.http.get<any>(
      `${this.apiUrl}/active/${userId}`
    );
  }

}
