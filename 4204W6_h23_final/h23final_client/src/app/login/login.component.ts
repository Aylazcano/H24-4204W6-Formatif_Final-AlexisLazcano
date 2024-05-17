import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginUsername : string = "";
  loginPassword : string = "";

  registerUsername : string = "";
  registerPassword : string = "";
  registerPasswordConfirm : string = "";

  constructor(public authService : AuthService) { }

  login(){
    if(this.loginUsername && this.loginPassword){
      this.authService.loginRequest(this.loginUsername, this.loginPassword);
    }
  }

  register(){
    if(this.registerUsername && this.registerPassword && this.registerPasswordConfirm){
      this.authService.registerRequest(this.registerUsername, this.registerPassword, this.registerPasswordConfirm);
    }
  }

  ngOnInit() {
  }

}
