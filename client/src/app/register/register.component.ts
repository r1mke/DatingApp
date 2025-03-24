import { Component, inject, input, Input, output, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  @ViewChild('registerForm') registerForm!: NgForm;
  accountService = inject(AccountService)
  toastr = inject(ToastrService)
  model:any = {}
  usersFromHomeComponent = input.required<any>();
  cancelRegisterMode = output<boolean>();

  register(){
    this.accountService.register(this.model).subscribe({
      next: (res) => {
        console.log(res);
        this.cancel();
      },
      error: error => this.toastr.error(error.error),
    }
    );
  }

  cancel(){
    this.cancelRegisterMode.emit(false);
  }
}
