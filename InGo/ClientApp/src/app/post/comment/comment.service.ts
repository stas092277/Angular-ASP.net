import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient} from '@angular/common/http';
import { comment} from './Itcomment';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  constructor(private http: HttpClient) { }

  getDataComment(id: number): Observable<comment[]> {
    return this.http.get<comment[]>('api/Comments/post/' + id, { responseType: 'json'});
  }


}
