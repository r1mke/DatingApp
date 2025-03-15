import { Component, inject, OnInit } from '@angular/core';
import { RegisterComponent } from "../register/register.component";
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  private http = inject (HttpClient);
  registerMode = false;
  users: any;

  ngOnInit(): void {
      this.getUsers();
  }

  registerToggle(){
    this.registerMode = !this.registerMode
  }

  getUsers(){
    this.http.get('http://localhost:5000/api/users').subscribe({
      next: (response) => {
        this.users = response;
        console.log(this.users);
      },
      error: (error) => {
        console.log("Error", error);
      }
    });
  }


  cancelRegisterMode(event: boolean){
    this.registerMode = event;
  }
}
