import { Injectable } from '@angular/core';
import {BehaviorSubject, Observable, Subject} from 'rxjs';
import { HttpClient} from '@angular/common/http';
import {EventEmitter} from '@angular/core';
import { Post } from '../post/Itpost';


@Injectable({
  providedIn: 'root'
})

export class PostsService {

  posts: EventEmitter<Post[]> = new EventEmitter();
  private subject = new Subject<any>();


  constructor(private http: HttpClient) { }

  getDataAllPosts() {
    return this.http.get<Post[]>('api/posts');
  }

  getData() {
    return this.http.get<Post[]>('api/posts');
  }


}
