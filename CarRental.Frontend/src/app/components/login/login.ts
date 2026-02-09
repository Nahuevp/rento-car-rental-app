import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class LoginComponent {

  username = '';
  password = '';
  error: string | null = null;
  loading = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  onSubmit(): void {
    this.loading = true;
    this.error = null;

    this.authService.login(this.username, this.password)
      .subscribe({
        next: (user: any) => {
          this.authService.saveUser(user);
          this.router.navigate(['/']); // o donde corresponda
        },
        error: () => {
          this.error = 'Usuario o contraseña incorrectos';
        }
      });

  }
}
