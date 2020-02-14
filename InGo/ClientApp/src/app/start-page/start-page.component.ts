import { Component, OnInit } from '@angular/core';
import { LoginModel} from './Classes/LoginModel';
import { HttpErrorResponse} from '@angular/common/http';
import { RegisterModel } from './Classes/RegisterModel';
import { AccountService} from './account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-start-page',
  templateUrl: './start-page.component.html',
  styleUrls: ['./start-page.component.sass']
})

export class StartPageComponent implements OnInit {

  constructor(private accountService: AccountService, private route: Router) { }
    LogOut() {
      localStorage.removeItem('token');
      this.route.navigateByUrl('login');
    }


  ngOnInit() {
  }

}
