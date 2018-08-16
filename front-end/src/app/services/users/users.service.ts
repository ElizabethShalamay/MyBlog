import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from '../../../../node_modules/rxjs';
import { User } from '../../models/user';
import { AccountService } from '../account/account.service';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  baseUrl = "api/users";

  constructor(private httpClient: HttpClient,
    private accService: AccountService) { }

  getUsers(page:number, url:string = ""): Observable<User[]> {
    url = url == "" ? `${this.baseUrl}?page=${page}`: `${url}?page=${page}`;
    return this.httpClient.get<User[]>(url, { headers: this.accService.getAuthHeaders() });
  }
  getUser(id: string): Observable<User> {
    let url = `${this.baseUrl}/${id}`;
    return this.httpClient.get<User>(url, { headers: this.accService.getAuthHeaders() });
  }

  deleteUser(id: string): Observable<User> {
    let url = `${this.baseUrl}/${id}`;
    return this.httpClient.delete<User>(url, { headers: this.accService.getAuthHeaders() });
  }
}
