using UnityEngine;
using System.Collections;

public class NetworkMessage
{
  public int EntityTypeId { get; set; } 
  public int EntityId { get; set; }
  public int PropertyId { get; set; }

  public NetworkMessage(int typeId, int entityId, int propertyId)
  {
    EntityTypeId = typeId;
    EntityId = entityId;
    PropertyId = propertyId;
  }

  public static NetworkMessage Instantiate(int typeId, int entityId, int propertyId)
  {
    return new NetworkMessage(typeId, entityId, propertyId);
  }
}

                                                                   