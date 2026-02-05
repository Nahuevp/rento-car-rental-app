import { Routes } from '@angular/router';
import { CarListComponent } from './components/car-list/car-list.component';
import { RentalCreateComponent } from './components/rental-create/rental-create.component';
import { RegisterComponent } from './components/register/register';
import { LoginComponent } from './components/login/login';
import { authGuard } from './guards/auth.guard';
import { MyReservationsComponent } from './components/my-reservations/my-reservations.component';

export const routes: Routes = [
  { path: '', component: CarListComponent },

  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },

  {
    path: 'my-reservations',
    component: MyReservationsComponent,
    canActivate: [authGuard]
  },

  {
    path: 'rental/:id',
    component: RentalCreateComponent,
    canActivate: [authGuard]
  },

  { path: '**', redirectTo: '' }
];
