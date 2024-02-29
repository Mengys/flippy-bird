using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private const float CAMERA_ORTHO_SIZE = 50f;
    private const float PIPE_WIDTH = 10.4f;
    private const float PIPE_HEAD_HEIGHT = 5f;
    private const float PIPE_MOVEMENT_SPEED = 30f;
    private const float PIPE_SPAWN_X_POSITION = +120f;
    private const float GROUND_DESTROY_X_POSITION = -200f;
    private const float CLOUD_DESTROY_X_POSITION = -160f;
    private const float CLOUD_X_DISTANCE = 140f;
    private const float CLOUD_SPEED = 10f;

    private static Level instance;

    public static Level GetInstance() {
        return instance;
    }

    private List<Transform> groundList;
    private List<Transform> cloudList;
    private List<Pipe> pipeList;
    private int pipesPassedCount;
    private int pipesSpawned;
    private int cloudsSpawned;
    private float gapSize;
    private float pipeDistance;
    private State state;

    public enum Difficulty {
        Easy,
        Medium,
        Hard,
        Imposible,
    }

    private enum State {
        WaitingToStart,
        Playing,
        BirdDied,
    }

    private void Awake() {
        instance = this;
        SpawnInitialGround();
        pipeList = new List<Pipe>();
        SetDifficulty(Difficulty.Easy);
        state = State.WaitingToStart;
        cloudList = new List<Transform>();
    }

    private void Start() {
        SpawnInitialPipes();
        SpawnInitialClouds();
        Bird.GetInstance().OnDied += Bird_OnDied;
        Bird.GetInstance().OnStartedPlaying += Bird_OnStartedPlaying;
    }

    private void Bird_OnStartedPlaying(object sender, System.EventArgs e) {
        state = State.Playing;
    }

    private void Bird_OnDied(object sender, System.EventArgs e) {
        state = State.BirdDied;
    }

    private void Update() {
        if (state == State.Playing) {
            HandleScore();
            HandlePipeSpawning();
            HandleGround();
            HandleClouds();
        }
    }

    private void SpawnInitialClouds() {

        SpawnCloud();
        SpawnCloud();
        SpawnCloud();
        SpawnCloud();
        SpawnCloud();
    }

    private Transform GetCloudPrefabTransform() {
        switch (Random.Range(0, 3)) {
        default:
        case 0:return GameAssets.GetInstance().pfCloud_1;
        case 1:return GameAssets.GetInstance().pfCloud_2;
        case 2:return GameAssets.GetInstance().pfCloud_3;
        }
    }

    private void SpawnCloud() {
        float mostXPosition = -240f;
        foreach(Transform transform in cloudList) {
            if (transform.position.x > mostXPosition) {
                mostXPosition = transform.position.x;
            }
        }

        Transform cloudTransform;
        float cloudY = 30f;
        cloudTransform = Instantiate(GetCloudPrefabTransform(), new Vector3(mostXPosition + CLOUD_X_DISTANCE, cloudY, 0), Quaternion.identity);
        cloudList.Add(cloudTransform);
        cloudsSpawned++;
    }

    private void HandleClouds() {
        for (int i = 0; i<cloudList.Count; i++) {
            Transform cloudTransform = cloudList[i];
            cloudTransform.position += new Vector3(1, 0, 0) * CLOUD_SPEED * Time.deltaTime;

            if (cloudTransform.position.x < Bird.GetInstance().GetXPosition() + CLOUD_DESTROY_X_POSITION ) {
                Destroy(cloudTransform.gameObject);
                cloudList.RemoveAt(i);
                SpawnCloud();
            }
        }
    }

    private void SpawnInitialGround() {
        groundList = new List<Transform>();
        Transform groundTransform;
        float groundY = -48f;
        float groundWidth = 192f;
        groundTransform = Instantiate(GameAssets.GetInstance().pfGround, new Vector3(-20f, groundY, 0), Quaternion.identity);
        groundList.Add(groundTransform);
        groundTransform = Instantiate(GameAssets.GetInstance().pfGround, new Vector3(-20f + groundWidth, groundY, 0), Quaternion.identity);
        groundList.Add(groundTransform);
        groundTransform = Instantiate(GameAssets.GetInstance().pfGround, new Vector3(-20f + groundWidth * 2, groundY, 0), Quaternion.identity);
        groundList.Add(groundTransform);
    }

    private void HandleGround() {
        foreach (Transform groundTransform in groundList) {
            if (groundTransform.position.x < Bird.GetInstance().GetXPosition() + GROUND_DESTROY_X_POSITION) {

                float groundWith = 192f;
                groundTransform.position = new Vector3(groundTransform.position.x + groundWith * 3, groundTransform.position.y, groundTransform.position.z);
            }
        }
    }

    private void SpawnInitialPipes() {
        float heightEdgeLimit = 10f;
        float minHeight = gapSize * .5f + heightEdgeLimit;
        float totalHeight = CAMERA_ORTHO_SIZE * 2;
        float maxHeight = totalHeight - gapSize * .5f - heightEdgeLimit;

        float height = Random.Range(minHeight, maxHeight);
        CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION);
        height = Random.Range(minHeight, maxHeight);
        CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION + pipeDistance * pipesSpawned);
        height = Random.Range(minHeight, maxHeight);
        CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION + pipeDistance * pipesSpawned);
        height = Random.Range(minHeight, maxHeight);
        CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION + pipeDistance * pipesSpawned);
        height = Random.Range(minHeight, maxHeight);
        CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION + pipeDistance * pipesSpawned);
        height = Random.Range(minHeight, maxHeight);
        CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION + pipeDistance * pipesSpawned);
        height = Random.Range(minHeight, maxHeight);
        CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION + pipeDistance * pipesSpawned);
        height = Random.Range(minHeight, maxHeight);
        CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION + pipeDistance * pipesSpawned);
        height = Random.Range(minHeight, maxHeight);
        CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION + pipeDistance * pipesSpawned);
        height = Random.Range(minHeight, maxHeight);
        CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION + pipeDistance * pipesSpawned);
    }

    private void HandlePipeSpawning() {
        if (pipesSpawned - pipesPassedCount < 5) {

            float heightEdgeLimit = 10f;
            float minHeight = gapSize * .5f + heightEdgeLimit;
            float totalHeight = CAMERA_ORTHO_SIZE * 2;
            float maxHeight = totalHeight - gapSize * .5f - heightEdgeLimit;

            float height = Random.Range(minHeight, maxHeight);
            float mostXPosition = 0f;
            foreach (Pipe pipe in pipeList) {
                if (mostXPosition < pipe.GetXPosition()) mostXPosition = pipe.GetXPosition();
            }

            CreateGapPipes(height, gapSize, mostXPosition + pipeDistance);
        }
    }

    private void HandleScore() {
        for (int i=0; i<pipeList.Count; i++) {
            Pipe pipe = pipeList[i];
            bool isToTheLeftOfBird = pipe.GetXPosition() < Bird.GetInstance().GetXPosition();

            if (isToTheLeftOfBird && pipe.IsBottom() && !pipe.GetPassed()) {
                pipesPassedCount++;
                pipe.Passed();
                SoundManager.PlaySound(SoundManager.Sound.Score);
            }
            if (pipe.GetXPosition() < Bird.GetInstance().GetXPosition() - 200f) {
                pipe.DestroySelf();
                pipeList.Remove(pipe);
                i--;
            }
        }
    }

    private void SetDifficulty(Difficulty difficulty) {
        switch (difficulty) {
        case Difficulty.Easy:
            gapSize = 50f;
            pipeDistance = 100f;
            break;
        case Difficulty.Medium:
            gapSize = 43f;
            pipeDistance = 90f;
            break;
        case Difficulty.Hard:
            gapSize = 36f;
            pipeDistance = 80f;
            break;
        case Difficulty.Imposible:
            gapSize = 30f;
            pipeDistance = 70f;
            break;
        }
    }

    private Difficulty GetDifficulty() {
        if (pipesSpawned >= 60) return Difficulty.Imposible;
        if (pipesSpawned >= 40) return Difficulty.Hard;
        if (pipesSpawned >= 20) return Difficulty.Medium;
        return Difficulty.Easy;
    }

    private void CreateGapPipes(float gapY, float gapSize, float xPosition) {
        CreatePipe(gapY - gapSize * .5f, xPosition, true);
        CreatePipe(CAMERA_ORTHO_SIZE * 2f - gapY - gapSize * .5f, xPosition, false);
        pipesSpawned++;
        SetDifficulty(GetDifficulty());
    }

    private void CreatePipe(float height, float xPosition, bool createBottom) {
        
        //Set up Pipe head
        Transform pipeHead = Instantiate(GameAssets.GetInstance().pfPipeHead);
        float pipeHeadYPosition;
        if (createBottom) {
            pipeHeadYPosition = -CAMERA_ORTHO_SIZE + height - PIPE_HEAD_HEIGHT * .5f + .2f;
        } else {
            pipeHeadYPosition = CAMERA_ORTHO_SIZE - height + PIPE_HEAD_HEIGHT * .5f - .2f;
        }
        pipeHead.position = new Vector3(xPosition, pipeHeadYPosition);

        //Set up Pipe body
        Transform pipeBody = Instantiate(GameAssets.GetInstance().pfPipeBody);
        float pipeBodyYPosition;
        if (createBottom) {
            pipeBodyYPosition = -CAMERA_ORTHO_SIZE;
        } else {
            pipeBodyYPosition = CAMERA_ORTHO_SIZE;
            pipeBody.localScale = new Vector3(1, -1, 1);
        }
        pipeBody.position = new Vector3(xPosition, pipeBodyYPosition);

        SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodySpriteRenderer.size = new Vector2(PIPE_WIDTH, height);

        BoxCollider2D pipeBodyBoxCollider2D = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyBoxCollider2D.size = new Vector2(PIPE_WIDTH, height);
        pipeBodyBoxCollider2D.offset = new Vector2(0f, height * 0.5f);

        Pipe pipe = new Pipe(pipeHead, pipeBody, createBottom);
        pipeList.Add(pipe);
    }

    public int GetPipesSpawned() {
        return pipesSpawned;
    }

    public int GetPipesPassedCount() {
        return pipesPassedCount;
    }


    private class Pipe {

        private Transform pipeHeadTransform;
        private Transform pipeBodyTransform;
        private bool isBottom;
        private bool passed;

        public Pipe(Transform pipeHeadTransform, Transform pipeBodyTransform, bool isBottom) {
            this.pipeHeadTransform = pipeHeadTransform;
            this.pipeBodyTransform = pipeBodyTransform;
            this.isBottom = isBottom;
            passed = false;
        }

        public void Move() {
            pipeHeadTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVEMENT_SPEED * Time.deltaTime;
            pipeBodyTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVEMENT_SPEED * Time.deltaTime;
        }

        public float GetXPosition() {
            return pipeHeadTransform.position.x;
        }

        public bool IsBottom() {
            return isBottom;
        }

        public void DestroySelf() {
            Destroy(pipeHeadTransform.gameObject);
            Destroy(pipeBodyTransform.gameObject);
        }

        public void Passed() {
            passed = true;
        }

        public bool GetPassed() {
            return passed;
        }
    }
}
