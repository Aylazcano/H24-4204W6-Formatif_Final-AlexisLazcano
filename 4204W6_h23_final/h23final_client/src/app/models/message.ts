import { Reaction } from './reaction';
export class Message{
    constructor(
        public id : number,
        public text : string,
        public sentAt : Date,
        public userName : string,
        public reactions : Reaction[]
    ){}
}