import { Component, OnInit } from '@angular/core';
import { UserService } from '../../_services/user.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {

  users: any = [];
  constructor(private _userService: UserService) {
    this.getUsers();

  }

  ngOnInit(): void {
  }

  getUsers() {
    this._userService.getUsers().subscribe((resp: any) => {
      this.users = resp.users;
    });
  }
  
  deleteUser(id: string, userName: string) {
    if (confirm('¿Está seguro de eliminar al usuario ' + userName)) {
      this._userService.removeUser(id).subscribe(() => {
        this.getUsers()
      },
        (er) => {
          console.log('error');
        })
    }
  }

}
