import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient} from '@angular/common/http';
import { Post } from '../../post/Itpost';

@Injectable({
  providedIn: 'root'
})
export class TagsServiceService {



  constructor(private http: HttpClient) { }

  getDataTopTags(): Observable<string[]> {
    return this.http.get<string[]>('api/tags/top/2');
  }

  getDataPostByTags(id): Observable<string[]> {
    return this.http.get<string[]>('api/tags/' + id);
  }

  getDataAllPosts(): Observable<Post[]> {
    return this.http.get<Post[]>('api/posts');
  }


}
