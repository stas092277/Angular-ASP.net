import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Post } from 'src/app/post/Itpost';
import { AccountService } from 'src/app/start-page/account.service';
import { UserSortingService } from './user-sorting.service';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-user-sort-table',
  templateUrl: './user-sort-table.component.html',
  styleUrls: ['./user-sort-table.component.sass']
})
export class UserSortTableComponent implements OnInit {

  postList: Post[];
  categorys = CATEGORYS;
  selectedCategory = CATEGORYS[0];
  @Output() outPostLost = new EventEmitter();

  constructor(
    private accountService: AccountService,
    private userSortingService: UserSortingService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
  }

  onSelect(category: Category): void {
    const id = +this.route.snapshot.paramMap.get('id');
    this.selectedCategory = category;

    switch(category.name) {
      case 'Мои' : {
        this.userSortingService.getDataUserPostsById(id).subscribe(data =>{
          this.postList = data;
          this.outPostLost.emit(this.postList);

        },
          err => console.log('HTTP error', err.message));
        break;
      }
      default: {
        this.userSortingService.getDataUserSavesPost(id).subscribe(data =>{
          this.postList = data;
          this.outPostLost.emit(this.postList);
        },
          err => console.log('HTTP error', err.message));
        break;
      }
    }
  }
}

class Category {
  name: string;
}

const CATEGORYS: Category[] =
  [
    { name: 'Мои' },
    { name: 'Сохраненные' }
  ];
