import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient} from '@angular/common/http';
import { Post } from '../post/Itpost';

@Injectable({
  providedIn: 'root'
})
export class AllMainPostsService {

  constructor(private http: HttpClient) { }

  getDataAllPosts(): Observable<Post[]> {
    return this.http.get<Post[]>('api/posts');
  }
}
