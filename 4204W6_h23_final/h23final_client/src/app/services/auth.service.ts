import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { lastValueFrom } from 'rxjs';

const domain = "https://localhost:7004";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(public httpClient : HttpClient) { }

  async loginRequest(usernameInput : string, passwordInput : string) : Promise<void> {
    let loginDTO = {
      username : usernameInput,
      password : passwordInput
    };
    let x = await lastValueFrom(this.httpClient.post<any>(domain + "/api/Users/Login", loginDTO));
    console.log(x);
    localStorage.setItem("token", x.token);
  }

  async registerRequest(usernameInput : string, passwordInput : string, passwordConfirmInput : string) : Promise<void>{
    let registerDTO = {
      username : usernameInput,
      password : passwordInput,
      passwordConfirm : passwordConfirmInput
    };
    let x = await lastValueFrom(this.httpClient.post<any>(domain + "/api/Users/Register", registerDTO))
    console.log(x);
  }

}
