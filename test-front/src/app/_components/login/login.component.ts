import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { GoogleLoginProvider, SocialAuthService } from 'angularx-social-login';
import { AuthService } from '../../_services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private _socialAuthService: SocialAuthService,
    private _authService: AuthService,
    private router: Router) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required,Validators.minLength(8)]]
    });
  }

  ngOnInit(): void {
  }

  signInWithGoogle(): void {
    this._socialAuthService.signIn(GoogleLoginProvider.PROVIDER_ID);
  }

  signOut(): void {
    this._authService.signOut();
  }
  signIn() {
    const val = this.form.value;
    const creds = { email: val.email, password: val.password }
    if (this.form.valid) {
      this._authService.signIn(creds)
        .subscribe(
          (resp: any) => {
            this._authService.processToken(resp.access_token)
            this.router.navigateByUrl('/users');
          }
        );
    }
  } 


}
