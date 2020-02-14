import { Component, OnInit } from '@angular/core';
import { HttpErrorResponse} from '@angular/common/http';
import { RegisterModel } from '../../Classes/RegisterModel';
import { AccountService} from '../../account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {
  RegUser: RegisterModel = new RegisterModel();

  constructor(private accountService: AccountService, private route: Router, private toastr: ToastrService) { }

  RegisterSubmit() {
    if (!(this.RegUser.Email && this.RegUser.FirstName && this.RegUser.LastName && this.RegUser.Password && this.RegUser.PasswordConfirm) ){
      this.toastr.error('Не все поля заполнены');
    } else if (!this.RegUser.Email.includes('@gmail')) {
      this.toastr.error('Недопустимый домен почты');
    } else if (this.RegUser.Password !== this.RegUser.PasswordConfirm) {
      this.toastr.error('Пароли не совпадают');
    } else if (this.RegUser.Password.length < 8) {
      this.toastr.error('Слишком короткий пароль');
    } else if (!this.RegUser.Password.match(/\d/)) {
      this.toastr.error('Пароль должен содержать цифру');
    } else if (!this.RegUser.Password.match(/[A-Z]/)) {
      this.toastr.error('Пароль должен содержать заглавную букву');
    } else {
        this.accountService.register(this.RegUser).subscribe( _ => {
            this.toastr.success('Вы зарегистрировались');
            this.route.navigateByUrl('/authorization/login');
          },
          (errorResponse: HttpErrorResponse) => {
             this.toastr.error('Ошибка регистрации. Возможно такйо пользователь уже загеристрирован');
          });
        }
  }
  ngOnInit() {
  }

}
