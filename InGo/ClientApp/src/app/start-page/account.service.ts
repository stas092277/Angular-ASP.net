import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegisterModel } from './Classes/RegisterModel';
import { Observable } from 'rxjs';
import { LoginModel } from './Classes/LoginModel';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private registerUrl = 'api/account/register';
  private loginUrl = 'api/account/login';
  private logoutUrl = 'api/account/logout';
  private UserProfileUrl = 'api/account/profile';
  private UploadPhotoUrl = 'api/files/uploadFile';
  private UpdateUserUrl = 'api/users/';

  isUserLogged: Subject<boolean> = new Subject<boolean>();

  constructor(private http: HttpClient) { }

  register(user: RegisterModel) {
   return this.http.post(this.registerUrl, user,
    {
       responseType: 'json'
    }
   );
  }

  login(user: LoginModel) {
    return this.http.post(this.loginUrl, user, {
      responseType: 'json'
    });
  }

  GetUserProfile() {
    return this.http.get(this.UserProfileUrl);
  }

  getRole()  {
    const payLoad = JSON.parse(window.atob(localStorage.getItem('token').split('.')[1]));
    return payLoad.role;
  }

  roleMatch(allowedRoles): boolean {
    let isMatch = false;
    const payLoad = JSON.parse(window.atob(localStorage.getItem('token').split('.')[1]));
    const userRole = payLoad.role;
    allowedRoles.forEach(element => {
      if (userRole === element) {
        isMatch = true;
        return false;
      }
    });
    return isMatch;
  }

  uploadPhoto(formData) {
    return this.http.post(this.UploadPhotoUrl, formData, { responseType: 'text'})
  }

  updateUserPhoto(url, currentUser) {
    currentUser.imgUrl = url;
    return this.http.post(this.UpdateUserUrl + currentUser.id + '/update', currentUser, {
      responseType: 'json'
   });
  }


  updateUserAboutInfo(info, currentUser) {
    currentUser.about = info;
    return this.http.post(this.UpdateUserUrl + currentUser.id + '/update', currentUser, {
      responseType: 'json'
    });
  }

}
