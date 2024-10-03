import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { FormGroup, FormControl, FormBuilder, Validators } from "@angular/forms";
import { AuthService } from '../../../../core/services/auth.service';
// import {CookieService} from 'ngx-cookie-service';

@Component({
    selector: "app-login",
    templateUrl: "./login.component.html",
    styleUrls: ["./login.component.scss"]
})

export class LoginComponent {
    loginForm: FormGroup;
    errorMessage: string = '';

    constructor(
        private formBuilder: FormBuilder, 
        private router: Router,
        private authService: AuthService,
        //private cookieService: CookieService
      ) {

      // Initialize the form using FormBuilder
      this.loginForm = this.formBuilder.group({
        username: ['', [Validators.required]],
        password: ['', [Validators.required, Validators.minLength(3)]]
      });
    }
    onSumbit(): void {
       const { username, password } = this.loginForm.value;

       this.authService.login(username, password).subscribe({
        next: (v) => {
          console.log('vvvv', v);
         // this.cookieService.set('authToken', 'Hello World');
         this.authService.setToken(v.token);
          setTimeout(() => {
            this.router.navigate(['/']);
           }, 1000);
        },
        error: err => {
          console.log('errxxx', err);
          
          this.errorMessage = 'Invalid username or password';
        }
      });
    }
};