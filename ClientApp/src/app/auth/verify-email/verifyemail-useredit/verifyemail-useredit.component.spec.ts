import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerifyemailUsereditComponent } from './verifyemail-useredit.component';

describe('VerifyemailUsereditComponent', () => {
  let component: VerifyemailUsereditComponent;
  let fixture: ComponentFixture<VerifyemailUsereditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerifyemailUsereditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerifyemailUsereditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
