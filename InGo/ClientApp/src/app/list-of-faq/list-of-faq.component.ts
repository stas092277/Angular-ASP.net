import { Component, OnInit } from '@angular/core';
import { AccountService } from '../start-page/account.service';
import { FaqSortService } from './faq-sort.service';
import { faqPost } from './feed-faq/ItFaqPost';

@Component({
  selector: 'app-list-of-faq',
  templateUrl: './list-of-faq.component.html',
  styleUrls: ['./list-of-faq.component.sass']
})
export class ListOfFaqComponent implements OnInit {

  postList: faqPost[];
  defaultValue = '';
  userDetails;


  TOPICS: Topic[] ;
  selectedTopic: Topic;

  constructor(
    private accountService: AccountService,
    private faqSortService: FaqSortService
    ) { }

  ngOnInit() {
      this.faqSortService.getDataFaqCategory().subscribe(data => {
        this.TOPICS = data
        this.onSelect((this.TOPICS[0]));
      },
        err => console.log('HTTP error', err.message));
  }

  onSelect(topic: Topic): void {
    this.selectedTopic = topic;
    this.faqSortService.getDataFaqByCategoryPosts(topic.id).subscribe(data => this.postList = data,
      err => console.log('HTTP error', err.message));
  }
}

class Topic {
  name: string;
  id: number;
}
