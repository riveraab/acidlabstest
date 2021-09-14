import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { IUser } from '../_interfaces/user.inteface';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private _http: HttpClient) { }

  getUsers() {
    return this._http.get(`${environment.api_url}user`);
  }

  getUser(id: string) {
    return this._http.get(`${environment.api_url}user/${id}`);
  }
  removeUser(id: string) {
    return this._http.delete(`${environment.api_url}user/${id}`);
  }

  addUser(user: IUser){
    return this._http.post(`${environment.api_url}user/`, user);
  }

  editUser(user: IUser){
    return this._http.put(`${environment.api_url}user/${user.id}`, user);
  }

}
