import { Component, OnInit } from '@angular/core';
import { LoginModel} from '../../Classes/LoginModel';
import { HttpErrorResponse} from '@angular/common/http';
import { AccountService} from '../../account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  LogUser: LoginModel = new LoginModel();
  registerFeedback: string;

  constructor(private accountService: AccountService, private route: Router, private toastr: ToastrService) { }

  ngOnInit() {
  }

  LoginSubmit() {
    this.accountService.login(this.LogUser).subscribe((res: any) => {
      localStorage.setItem('token', res.token);
      this.toastr.success('Вход выполнен успешно');
      this.accountService.isUserLogged.next(true);
      this.route.navigateByUrl('/posts');
    }, (errorResponse: HttpErrorResponse) => {
      this.toastr.error('Ошибка входа!');
    });
  }

}
