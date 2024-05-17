import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ChatService } from '../services/chat.service';
import { Channel } from '../models/channel';
import { Message } from '../models/message';
import { Reaction } from '../models/reaction';

@Component({
  selector: 'app-channels',
  templateUrl: './channels.component.html',
  styleUrls: ['./channels.component.css']
})
export class ChannelsComponent implements OnInit {

  channelList : Channel[] = [];
  selectedChannel : Channel | null = null;

  messageList : Message[] = [];
  selectedMessage : Message | null = null;

  messageTextInput : string = "";

  reactionOverlayOn : boolean = false;
  @ViewChild("newReactionFile", {static:false}) newNeactionFile ?: ElementRef;

  constructor(public chatService : ChatService) { }

  ngOnInit() {
    this.getChannel();
  }

  selectChannel(channel : Channel){
    this.selectedChannel = channel;
    this.getChannelMessages();  
  }

  selectMessage(overlayOn : boolean, message : Message | null){
    this.reactionOverlayOn = overlayOn;
    this.selectedMessage = message;
  }

  async getChannel(){
    this.channelList = await this.chatService.getChannelRequest();
  }

  // ███ CETTE FONCTION EST À COMPLÉTER ███
  async postChannel(){
    
    // Allo 👋
    // Ce n'est pas grave si jamais il faut réactualiser la page pour voir le nouveau channel.

  }

  async getChannelMessages(){
    if(this.selectedChannel){
      this.messageList = await this.chatService.getChannelMessagesRequest(this.selectedChannel.id);
    }
  }

  async postMessage(){
    if(this.messageTextInput && this.selectedChannel){
      let newMessage = await this.chatService.postMessageRequest(this.messageTextInput, this.selectedChannel.id);
      this.messageList.push(newMessage);
      this.messageTextInput = "";
    }
  }

  async deleteMessage(messageId : number){
    // Si la suppression du message réussit...
    if(await this.chatService.deleteMessageRequest(messageId)){

      // Trouver le message dans la lite pour le supprimer
      for(let i = this.messageList.length - 1; i >= 0; i--){
        if(this.messageList[i].id == messageId){
          this.messageList.splice(i, 1);
        }
      }
    }
  }

  async toggleReaction(reactionId : number, message : Message){
      let toggledReaction : Reaction | null = await this.chatService.toggleReactionRequest(reactionId);
      if(toggledReaction){
        for(let r of message.reactions){
          if(r.id == reactionId){
            r.quantity = toggledReaction.quantity;
          }
        }
      }
      else{
        for(let i = message.reactions.length - 1; i >= 0; i--){
          if(message.reactions[i].id == reactionId){
            message.reactions.splice(i, 1);
          }
        }
      }

  }

  async postReaction(){
    if(!this.selectedMessage){
      console.log("Aucun message sélectionné");
      return;
    }
    if(this.newNeactionFile == undefined){
      console.log("Input HTML non chargé.");
      return;
    }
    let file = this.newNeactionFile.nativeElement.files[0];
    if(file == null){
      console.log("Input HTML ne contient aucune image.");
      return;
    }
    let formData : FormData = new FormData();
    formData.append("imageReaction", file, file.name);
    let reaction : Reaction = await this.chatService.postReactionRequest(this.selectedMessage.id, formData);
    this.selectedMessage.reactions = [];
    this.selectedMessage.reactions.push(reaction);
    this.selectMessage(false, null);
  }

}
