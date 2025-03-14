import { Component, inject, OnInit  } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClient } from '@angular/common/http';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  private http = inject(HttpClient);

  ngOnInit() {
    this.http.get('http://localhost:5000/api/users').subscribe({
      next: (response) => {
        console.log("Ok", response);
      },
      error: (error) => {
        console.log("Error", error);
      }
    });
  }
}
