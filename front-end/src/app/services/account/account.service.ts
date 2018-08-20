import { Injectable, Output, EventEmitter } from '@angular/core';
import { Observable, EMPTY, throwError } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LoginModel } from '../../models/login-model';
import { TokenParams } from '../../models/token-params'
import { RegisterModel } from '../../models/register-model';
import { Router } from '@angular/router'
import { tap } from "rxjs/operators";

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
  bagRequest: boolean;

  constructor(private httpClient: HttpClient,
    private router: Router) { }

  signIn(loginModel: LoginModel): Observable<TokenParams> {
    const url = this.tokenUrl;
    var data = `grant_type=password&username=${loginModel.username}&password=${loginModel.password}`;
    var headers = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' });
    return this.httpClient.post<TokenParams>(url, data, { headers: headers })
      .pipe(
        tap(data => {
          sessionStorage.setItem("Authorization", data.access_token);

          this.authentication.userName = loginModel.username;
          this.authentication.isAuth = true;
        })
      );
  }

  register(registerModel: RegisterModel): Observable<RegisterModel> {
    const url = `${this.baseUrl}/register`;
    return this.httpClient.post<RegisterModel>(url, registerModel);
  }

  logOut() {
    sessionStorage.removeItem("Authorization");
    this.authentication.isAuth = false;
    this.getLoggedIn.emit(false);
    this.authentication.userName = "";
    this.router.navigate(['/login']);
  }

  getAuthHeaders(): HttpHeaders {
    var authData = sessionStorage.getItem("Authorization");
    if (authData) {
      var headers = new HttpHeaders();
      headers = headers.append('Authorization', `Bearer ${authData}`);
      return headers;
    }
  }
}
