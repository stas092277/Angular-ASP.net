import { Injectable } from '@angular/core';
import { Users } from './Itiuser';
import { HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';
import { Post } from '../post/Itpost';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }


  getDataUserById(id: number): Observable<Users> {
    return this.http.get<Users>( 'api/users/' + id);
  }

  getDataUserPostsById(id: number): Observable<Post[]> {
    return this.http.get<Post[]>('api/users/' + id + '/posts');
  }
}
