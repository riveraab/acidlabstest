import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent implements OnInit {
  form: FormGroup;
  constructor( private fb: FormBuilder,) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      name: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(8)]],
      password2: ['', [Validators.required, this.matchPasswords]]
    });
   }

  ngOnInit(): void {
  }

  matchPasswords() {
    if (this.form){
      console.log(this.form);
      const p1 = this.form.get('password')?.value || '';
      const p2 = this.form.get('password2')?.value || '';
      if (p1 == p2) {
        return null;
      }  
      return {
        mismatch: true
      };
    }
    return null;
  }

}
