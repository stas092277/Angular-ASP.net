import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient} from '@angular/common/http';
import { faqPost } from './feed-faq/ItFaqPost';


@Injectable({
  providedIn: 'root'
})
export class FaqSortService {

  constructor(private http: HttpClient) { }

  getDataFaqPosts(): Observable<faqPost[]> {
    return this.http.get<faqPost[]>('api/faq');
  }

  getDataFaqCategory(): Observable<Topic[]> {
    return this.http.get<Topic[]>('api/faqcategory');
  }

  getDataFaqByCategoryPosts(id: number): Observable<faqPost[]> {
    return this.http.get<faqPost[]>( 'api/faqcategory/' + id + '/faqposts');
  }
}

class Topic {
  name: string;
  id: number;
  description: any;
}
