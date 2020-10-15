import { Component } from '@angular/core';
import { UserService } from '../shared/user.service';

@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html'
})
export class CounterComponent {

  constructor(private service: UserService) { }

  userDetails;

  ngOnInit() {
    this.service.getUserProfile().subscribe(
      res => {
        this.userDetails = res;
        console.log(this.userDetails);
      },
      err => {
        console.error(err);
      },
    );
  }

  public currentCount = 0;

  public incrementCounter() {
    this.currentCount++;
  }
}
