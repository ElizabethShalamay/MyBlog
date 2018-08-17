import { Injectable, Output, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LoginModel } from '../../models/login-model';
import { TokenParams } from '../../models/token-params'
import { RegisterModel } from '../../models/register-model';
import { Router } from '@angular/router'

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  readonly baseUrl = '/api/account';
  authentication = {
    isAuth: false,
    userName: ""
  };

  @Output() getLoggedIn: EventEmitter<any> = new EventEmitter();


  private tokenUrl = '/Token';

  constructor(private httpClient: HttpClient,
    private router: Router) { }

  signIn(loginModel: LoginModel): Observable<TokenParams> {
    const url = this.tokenUrl;
    var data = `grant_type=password&username=${loginModel.username}&password=${loginModel.password}`;
    var headers = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' });
    return this.httpClient.post<TokenParams>(url, data, { headers: headers });
  }

  register(registerModel: RegisterModel): Observable<RegisterModel> {
    const url = `${this.baseUrl}/register`;
    return this.httpClient.post<RegisterModel>(url, registerModel);
  }

  logOut() {
    localStorage.removeItem("Authorization");
    this.authentication.isAuth = false;
    this.getLoggedIn.emit(false);
    this.authentication.userName = "";
    this.router.navigate(['/login']);
  }

  getAuthHeaders(): HttpHeaders {
    var authData = localStorage.getItem("Authorization");
    if (authData) {
      var headers = new HttpHeaders();
      headers = headers.append('Authorization', `Bearer ${authData}`);
      return headers;
    }
  }
}
