import { Component, OnInit } from '@angular/core';
import { Users } from './Itiuser';
import { UserService } from './user.service';
import { AccountService } from '../start-page/account.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Post } from '../post/Itpost';
import { HttpClient, HttpEventType, HttpErrorResponse } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { Route } from '@angular/compiler/src/core';
import { Location } from '@angular/common';


@Component({
  selector: 'app-user-about-panel',
  templateUrl: './user-about-panel.component.html',
  styleUrls: ['./user-about-panel.component.sass']
})
export class UserAboutPanelComponent implements OnInit {
  defaultValue = '';
  userDetails;
  user: Users;
  postUserList: Post[];
  boolEditInfo = false;

  constructor(
    private userService: UserService,
    private accountService: AccountService,
    private route: ActivatedRoute,
    private toastr: ToastrService,
  ) { }

  ngOnInit() {
    const id = +this.route.snapshot.paramMap.get('id');
    this.userService.getDataUserById(id).subscribe(data => {
      this.user = data;
      },
      err => console.log('HTTP error', err.message));

    this.userService.getDataUserPostsById(id).subscribe(data => this.postUserList = data,
      err => console.log('HTTP error', err.message));

    this.accountService.GetUserProfile().subscribe(res => {
      this.userDetails = res;
    }, err => { console.log(err); }, );
  }

  isMyProfile(user: Users) {
    if (user.id === this.userDetails.id) {
      return true;
    }
    return false;
  }

  public uploadPhoto = (files) => {
    if (files.length === 0) {
      return;
  }
    const fileToUpload = files[0] as File;
    const formData = new FormData();
    formData.append('file', fileToUpload);

    this.accountService.uploadPhoto(formData).subscribe((res: any) => {
      this.UpdateUserPhoto(res);
    });
  }

  UpdateUserPhoto(url) {
    this.accountService.updateUserPhoto(url, this.userDetails).subscribe((res: any) => {
      this.toastr.success('Фото загружено успешно');
      this.ngOnInit();
    }, (errorResponse: HttpErrorResponse) => {
      this.toastr.error('Ошибка загрузки фотографии');
    });
  }

  UpdateUserAbout(info) {
    this.accountService.updateUserAboutInfo(info, this.userDetails).subscribe((res: any) => {
      this.toastr.success('Иннформация онбновлена успешно');
      this.ngOnInit();
    }, (errorResponse: HttpErrorResponse) => {
      this.toastr.error('Ошибка обновления информации');
    });
  }

  receivePostFromTag($event: Post[]) {
    this.postUserList = $event;
  }
}



