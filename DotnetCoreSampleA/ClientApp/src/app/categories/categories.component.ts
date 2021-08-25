import { Component, ElementRef, Inject, OnInit, ViewChild } from '@angular/core';
import $ from 'jquery';
import 'bootstrap';
import { Categories } from './Categories';
import { CategoriesService } from './categories.service';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent implements OnInit {


  heroes: Categories[];
  editHero: Categories; // the hero currently being edited
  title = '';
  name = '';
  id = 0;
  message = '';
  edit = false;

  @ViewChild('exampleModal', { static: false }) exampleModal: ElementRef;
  @ViewChild('toast', { static: false }) toast: ElementRef;
  @ViewChild('namefiled', { static: false }) namefiled: ElementRef;
  @ViewChild('delbtn', { static: false }) delbtn: ElementRef;
  constructor(private heroesService: CategoriesService, @Inject('BASE_URL') baseUrl: string) {
    console.log("BaeURL:"+baseUrl);
  }
  ngOnInit() {
    this.getHeroes();
  }

  getHeroes(): void {
    this.heroesService.getHeroes()
      .subscribe(heroes => (this.heroes = heroes));
  }
  open2() {
    this.edit = false;
    this.title = 'Add Category';
    this.name = '';
    this.id = 0;

    $(this.exampleModal.nativeElement).modal('show');
    //this.delbtn.nativeElement = false;
  }
  add(newHero: Categories): void {
    this.editHero = undefined;
    //namex = namex.trim();
    //if (!namex) {
    //  return;
    //}

    //// The server will generate the id for this new hero
    //const newHero: User = {
    //  id: 1,
    //  name: namex
    //} as User;
    this.heroesService
      .addHero(newHero)
      .subscribe(hero => {
        this.heroes.push(hero);
        $(this.exampleModal.nativeElement).modal('hide');
        this.message = 'Category details added';
        $(this.toast.nativeElement).toast('show');
      });

  }

  open(person: Categories) {
    this.edit = true;
    this.title = 'Edit Category';
    this.name = person.cat_name;
    //this.email = person.email;
    this.id = person.cat_id;

    $(this.exampleModal.nativeElement).modal('show');
    // this.delbtn.nativeElement = true;
    // const modalRef = this.modalService.open(ModalComponent);
    //  const modalRef = this.modalService.open(ModalComponent);
    //  modalRef.componentInstance.title = 'About';
    // modalRef.componentInstance.name = person.name;
    //  modalRef.componentInstance.id = person.id;
    //  this.announce();
  }



  update(person) {
    if (this.edit) {
      //      console.log(person);
      this.heroesService
        .updateHero(person)
        .subscribe(hero => {
          //   console.log(hero);
          $(this.exampleModal.nativeElement).modal('hide');
          this.getHeroes();
          this.message = 'Category details updated';
          $(this.toast.nativeElement).toast('show');

          //  this.sendMessageToParent('messageToSendC');
          // replace the hero in the heroes list with update from server
          // const ix = hero ? this.person.findIndex(h => h.id === hero.id) : -1;
          // if (ix > -1) {
          //   this.person[ix] = hero;/   }
        });
      // this.editHero = undefined;
      // }
    } else {
      this.add(this.namefiled.nativeElement.value);
    }


  }
  closeModal() {
    $(this.exampleModal.nativeElement).modal('hide');
  }
  addX(name: string): void {
    name = name.trim();
    if (!name) { return; }
    this.heroesService.addHeroX({ cat_name: name } as Categories)
      .subscribe(hero => {
        //console.log(hero.email);
        this.heroes.push(hero);
        $(this.exampleModal.nativeElement).modal('hide');
        this.message = 'Category details added';
        $(this.toast.nativeElement).toast('show');
      });
  }


  delete(hero: Categories): void {
    console.log('size' + this.heroes.length);
    this.heroes = this.heroes.filter(h => h.cat_id !== hero.cat_id);
    console.log('size' + this.heroes.length);
    // this.heroes.re
    this.heroesService
      .deleteHero(hero.cat_id)
      .subscribe();
    $(this.exampleModal.nativeElement).modal('hide');
    // this.getHeroes();
    this.message = 'Category deleted';
    $(this.toast.nativeElement).toast('show');
    /*
    // oops ... subscribe() is missing so nothing happens
    this.heroesService.deleteHero(hero.id);
    */
  }

  //goToComponentB(people): void {
  //  this.router.navigate(['/peopledetails'], { state: { data: { people } } });
  //}
}
