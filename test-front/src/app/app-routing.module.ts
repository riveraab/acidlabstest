import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddUserComponent } from './_components/add-user/add-user.component';
import { LoginComponent } from './_components/login/login.component';
import { UserListComponent } from './_components/user-list/user-list.component';
import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'register', component: AddUserComponent },
  { path: 'users', component: UserListComponent, canActivate: [AuthGuard] },
  { path: 'user/:id', component: AddUserComponent, canActivate: [AuthGuard] },
  { path: 'user', component: AddUserComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }