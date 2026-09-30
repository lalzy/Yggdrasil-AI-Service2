// WorldService.cs

using Yggdrasil.Models.Entities;

namespace Yggdrasil.Services;

public class WorldService{
    public World Create(){
        return new World{Name="", Description=""};
    }
    public World Get(){
        return new World{Name="", Description=""};
    }
    public List<World> GetAll(){
        return [new World{Name="", Description=""}];
    }
    
    public World Update(){
        return new World{Name="", Description=""};
    }
    public void Delete(){
    }
}
