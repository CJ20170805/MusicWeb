import { NgModule } from "@angular/core";

import { CommonModule } from '@angular/common';

import { HomeComponent } from "./page/home/home.component";
import { AboutComponent } from "./page/about/about.component";

import { MainRoutingModule } from "./main.routing";
import { ReactiveFormsModule } from "@angular/forms";

@NgModule({
    imports: [MainRoutingModule, CommonModule, ReactiveFormsModule, HomeComponent, AboutComponent],
    exports: [],
    declarations: [],
    providers: [],
})

export class MainModule {}