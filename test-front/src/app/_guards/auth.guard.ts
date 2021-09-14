import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthService } from '../_services/auth.service';


@Injectable({
    providedIn: 'root'
  })
export class AuthGuard implements CanActivate {

  constructor(public _authService: AuthService, public router: Router) {}

  canActivate(): boolean {
    if (!this._authService.isLoggedIn()) {
      this.router.navigate(['login']);
      return false;
    }
    return true;
  }
}