import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';
import { Post } from 'src/app/post/Itpost';

@Injectable({
  providedIn: 'root'
})
export class UserSortingService {

  constructor(private http: HttpClient) { }

  private url = 'https://localhost:44327';

  getDataUserPostsById(id: number): Observable<Post[]> {
    return this.http.get<Post[]>(this.url + '/api/users/' + id + '/posts');
  }

  getDataUserSavesPost(id: number): Observable<Post[]> {
    return this.http.get<Post[]>(this.url + '/api/users/' + id + '/savedposts');
  }
}
