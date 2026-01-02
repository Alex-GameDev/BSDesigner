# Smart objects

Smart objects allows smart agents to attach/detach runtime generated behaviour, specified from world elements instead of the agents themselves.

Behaviour systems can execute an action of type `SmartObjectAction`. These actions should implement `GetSmartObject` method to specify how the smart object is retrieved and used.

Smart objects implements `ISmartObject` interface, which provides `RequestInteraction` method. This method creates an action using a template filled with an execution context provided by a behaviour system.

### Execution

1. BS execution reach SmartObject action.
2. On start: 
    - Retrieve smart object depending on the implementation.
    - Generates the action using the execution context.
    - Starts the action.
3. On update:
    - Ticks the action.
4. On stop:
    - Stops the action.


