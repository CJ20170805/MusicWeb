import { NgModule } from "@angular/core";

import { CommonModule } from '@angular/common';

import { LoginComponent } from "./page/login/login.component";
import {RegisterComponent} from "./page/register/register.component"

import { AuthRoutingModule } from "./auth.routing";
import { ReactiveFormsModule } from "@angular/forms";

@NgModule({
    imports: [AuthRoutingModule, CommonModule, ReactiveFormsModule],
    exports: [],
    declarations: [LoginComponent, RegisterComponent],
    providers: [],
})

export class AuthModule { }