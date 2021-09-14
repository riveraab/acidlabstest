import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { IUser } from 'src/app/_interfaces/user.inteface';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent implements OnInit {
  form: FormGroup;
  idEditing = '';
  userName = '';
  constructor(private fb: FormBuilder, private _userService: UserService, private router: Router, private _authService: AuthService, private activatedRoute: ActivatedRoute) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      name: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(8)]],
      password2: ['', Validators.required]
    }, { validators: this.checkPasswords });

    this.activatedRoute.paramMap.subscribe(x => {
      this.idEditing = x.get('id') || '';
      if (this.idEditing != '') {
        this._userService.getUser(this.idEditing).subscribe(
          (resp: any) => {
            this.form.get('email')?.setValue(resp.email);
            this.form.get('name')?.setValue(resp.name);
            this.userName = resp.name;
          },
          (err) => {

          }
        )
      }
    });
  }

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {

    });
  }

  checkPasswords: ValidatorFn = (group: AbstractControl): ValidationErrors | null => {
    let pass = group.get('password')?.value;
    let confirmPass = group.get('password2')?.value
    return pass === confirmPass ? null : { notSame: true }
  }

  save() {
    if (this.form.valid) {
      if (this.idEditing != '') {
        const user: IUser = {
          id: this.idEditing,
          name: this.form.get('name')?.value,
          email: this.form.get('email')?.value,
          password: this.form.get('password')?.value
        }
        this._userService.editUser(user).subscribe(
          () => {
            this.router.navigateByUrl('/users')
          },
          (err) => this.handleError(err)
        );
      } else {
        const user: IUser = {
          name: this.form.get('name')?.value,
          email: this.form.get('email')?.value,
          password: this.form.get('password')?.value
        }
        this._userService.addUser(user).subscribe(
          () => {
            if (this._authService.isLoggedIn()) {
              this.router.navigateByUrl('/users');
            } else {
              this.router.navigateByUrl('/login');
            }

          },
          (err) => this.handleError(err));
      }
    }

  }

  handleError(error: HttpErrorResponse) {
    console.log(error);
    const message = error.error?.Message || ''
    if (message != '') {
      window.alert(message);
    }
  }
}
