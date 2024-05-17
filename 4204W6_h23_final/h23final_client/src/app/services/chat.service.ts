import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { Channel } from '../models/channel';
import { Message } from '../models/message';
import { Reaction } from '../models/reaction';

const domain = "https://localhost:7004";

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  constructor(public httpClient : HttpClient) { }

  // ███ COMPLETÉ : Il n'y a aucune fonction pour poster un nouveau Channel pour le moment ███
  async postChannelRequest(channelName : string): Promise<any> {
    let newChannel = new Channel(0, channelName, []);
    let x = await lastValueFrom(this.httpClient.post<Channel>(domain + "/api/Channels/PostChannel", newChannel));
    console.log(x);
  }

  async getChannelRequest(): Promise<Channel[]> {
    let x : Channel[] = await lastValueFrom(this.httpClient.get<Channel[]>(domain + "/api/Channels/GetChannel"));
    console.log(x);
    return x;
  }

  async getChannelMessagesRequest(channelId : number): Promise<Message[]> {
    let x = await lastValueFrom(this.httpClient.get<Message[]>(domain + "/api/Messages/GetChannelMessages/" + channelId));
    console.log(x);
    return x;
  }

  async postMessageRequest(inputText : string, channelId : number): Promise<Message> {
    let messageDTO = {
      text : inputText,
      channelId : channelId
    };
    let x = await lastValueFrom(this.httpClient.post<Message>(domain + "/api/Messages/PostMessage", messageDTO));
    console.log(x);
    return x;
  }

  async deleteMessageRequest(messageId : number): Promise<boolean> {
    try{
      let x = await lastValueFrom(this.httpClient.delete<any>(domain + "/api/Messages/DeleteMessage/" + messageId));
      console.log(x);
    }
    catch{
      // Si la requête échoue
      return false;
    }
    // Si la requête réussit
    return true;
  }

  async toggleReactionRequest(reactionId : number): Promise<Reaction | null> {
    let x : Reaction | null = await lastValueFrom(this.httpClient.put<any>(domain + "/api/Reactions/ToggleReaction/" + reactionId, null));
    console.log(x);
    return x;
  }

  async postReactionRequest(messageId : number, formData : FormData): Promise<Reaction> {
    let x : Reaction = await lastValueFrom(this.httpClient.post<any>(domain + "/api/Reactions/PostReaction/" + messageId, formData));
    console.log(x);
    return x;
  }

}
