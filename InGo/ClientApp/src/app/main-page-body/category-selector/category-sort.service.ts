import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient} from '@angular/common/http';
import { Post } from '../../post/Itpost';

@Injectable({
  providedIn: 'root'
})
export class CategorySortService {


  constructor(private http: HttpClient) { }

  getDataAllPosts(): Observable<Post[]> {
    return this.http.get<Post[]>( 'api/posts');
  }

  getDataTopPosts(): Observable<Post[]> {
    return this.http.get<Post[]>( 'api/posts/top');
  }

  getDataUsefulPosts(): Observable<Post[]> {
    return this.http.get<Post[]>( 'api/posts/useful');
  }

}
