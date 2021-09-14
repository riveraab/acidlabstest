import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SocialAuthService, SocialUser } from 'angularx-social-login';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  user: SocialUser | null;
  loggedIn = false;

 
  constructor(private _socialAuthService: SocialAuthService, public _authService: AuthService, private router: Router) {
    this.user = null;
    this._socialAuthService.authState.subscribe((user: SocialUser) => {
      if (user) {
        this._authService.signInGoogle({ token: user.idToken }).subscribe((authToken: any) => {
          this._authService.processToken(authToken.access_token);
          this.loggedIn = true;
          this.router.navigateByUrl('/users');
        });
      }
      this.user = user;
    },
    (err) => {
      this.loggedIn = false;
    });
  }

  ngOnInit(): void {
    this.loggedIn = this._authService.isLoggedIn();
    console.log(this.loggedIn);
  }

  signout(){
    this.loggedIn = false;
    this.router.navigateByUrl('/login');
    this._authService.signOut();
  }
}

