import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';
import { comment } from '../comment/Itcomment';


@Injectable({
  providedIn: 'root'
})
export class EditCommentService {

  constructor(
    private http: HttpClient) { }



  postComment(comment: comment) {
    return this.http.post('api/comments', comment);
  }
}
