import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {NavComponent} from "@shared/components/nav/nav.component";

@Component({
  selector: 'app-content-layout',
  standalone: true,
  imports: [RouterOutlet, NavComponent],
  templateUrl: './content-layout.component.html',
  styleUrl: './content-layout.component.scss'
})
export class ContentLayoutComponent {

}
