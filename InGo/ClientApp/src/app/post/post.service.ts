import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Post} from './Itpost';
import { HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PostService {

constructor(private http: HttpClient) { }

  addPost(title: string, content: string, id: any) {

    const body = {
      Content: content,
      Title: title,
      AuthorId: id
    };

    return this.http.post('api/posts', body, {
      responseType: 'json'
    });
  }
  getDataPostById(id: number): Observable<Post> {
    return this.http.get<Post>('api/posts/' + id);
  }

  addCommentToPost(id: number, content: string) {
    const body = {
      Content: content
    };
    return this.http.post('api/posts/' + id + '/addComment', body, {responseType: 'json'});
  }

  deletePost(id: number) {
    return this.http.delete('api/posts/' + id, {responseType: 'json'});
  }


  like(id: number): Observable<Post> {
    return this.http.post<Post>(`api/posts/${id}/like`, {});
  }

  unlike(id: number): Observable<Post> {
    return this.http.post<Post>(`api/posts/${id}/unlike`, {});
  }

  save(id: number): Observable<Post> {
    return this.http.post<Post>(`api/posts/${id}/save`, {});
  }

  unsave(id: number): Observable<Post> {
    return this.http.post<Post>(`api/posts/${id}/unsave`, {});
  }
  addTags(tags: string[]) {
    return this.http.post(`api/tags/addsome`, tags );
  }

  addTagsToPost(id, tags: string[]) {
    return this.http.post(`api/posts/${id}/addTags`, tags);
  }
}
