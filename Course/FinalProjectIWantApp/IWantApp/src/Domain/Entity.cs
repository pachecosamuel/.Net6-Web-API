﻿using Flunt.Notifications;
using System.ComponentModel.DataAnnotations;

namespace IWantApp.Domain;

public class Entity : Notifiable<Notification>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string EditedBy { get; set; }
    public DateTime EditedOn { get; set; }

    public Entity()
    {
        Id = Guid.NewGuid();
    }
}
