import { Component, OnInit, Input } from '@angular/core';
import { AccountService} from '../../start-page/account.service';
import {Post} from '../../post/Itpost';

@Component({
  selector: 'app-feed-faq',
  templateUrl: './feed-faq.component.html',
  styleUrls: ['./feed-faq.component.sass']
})
export class FeedFaqComponent implements OnInit {
  isAdmin = false;

  @Input() faqData: Post[];

  constructor(private accountService: AccountService) { }

  ngOnInit() {
    const role = this.accountService.getRole();
    this.isAdmin = role === 'Admin';
  }
}
