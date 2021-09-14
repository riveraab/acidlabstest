import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SocialAuthService } from 'angularx-social-login';
import { environment } from 'src/environments/environment';
import { ILoginCredentials } from '../_interfaces/login-credentials.interface';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private _socialService: SocialAuthService, private _http: HttpClient) {
  }

  signIn(cred: ILoginCredentials) {
    return this._http.post(`${environment.api_url}auth/login`, cred);
  }

  signInGoogle(cred: any) {
    return this._http.post(`${environment.api_url}auth/login/google`, cred);
  }

  processToken(token: string) {
    sessionStorage.setItem('token', JSON.stringify(token));
    const helper = new JwtHelperService();
    const payload = helper.decodeToken(token);
    sessionStorage.setItem('userName', payload.sub);
  }

  signOut(): void {
    //remove token 
    sessionStorage.removeItem('token');
    sessionStorage.removeItem('userName');
    this._socialService.signOut();
  }

  isLoggedIn() {
    const value = sessionStorage.getItem('token') || ''
    const token = JSON.parse(value);
    if (token) {
      const helper = new JwtHelperService();
      return !helper.isTokenExpired(token);
    } else {
      return false;
    };
  }

  getUserName() {
    return sessionStorage.getItem('userName') || '';
  }
}
